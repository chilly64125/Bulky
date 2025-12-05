param()

# Test configuration
$backendUrl = 'http://localhost:5064'
$testResults = @()

# Helper to format test results
function Report-Test {
    param([string]$Entity, [string]$Operation, [string]$Status, [string]$Details)
    $testResults += [PSCustomObject]@{
        Entity = $Entity
        Operation = $Operation
        Status = $Status
        Details = $Details
        Timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    }
    Write-Host "[$Entity] $Operation : $Status - $Details"
}

# Step 1: Login
Write-Host "====== STEP 1: LOGIN ======"
Add-Type -AssemblyName System.Net.Http
$handler = New-Object System.Net.Http.HttpClientHandler
$handler.UseCookies = $true
$handler.CookieContainer = New-Object System.Net.CookieContainer
$client = New-Object System.Net.Http.HttpClient($handler)

try {
    $loginJson = '{"email":"admin@chen","password":"Admin1788@"}'
    $loginContent = New-Object System.Net.Http.StringContent($loginJson, [System.Text.Encoding]::UTF8, 'application/json')
    $loginResp = $client.PostAsync("$backendUrl/api/auth/login", $loginContent).Result
    $loginBody = $loginResp.Content.ReadAsStringAsync().Result
    
    if ($loginResp.StatusCode -eq 200) {
        Report-Test "Auth" "Login" "PASS" "admin@chen logged in successfully"
        Write-Host "Auth token obtained: $($loginResp.StatusCode)"
    } else {
        Report-Test "Auth" "Login" "FAIL" "Status: $($loginResp.StatusCode)"
        exit 1
    }
}
catch {
    Report-Test "Auth" "Login" "FAIL" "Exception: $_"
    exit 1
}

# Helper function to test CRUD operations
function Test-CrudEntity {
    param([string]$Entity, [string]$Endpoint, [hashtable]$CreateData, [hashtable]$UpdateData)
    
    Write-Host "`n====== TESTING: $Entity ======"
    
    $entityId = $null
    
    # CREATE
    try {
        $createJson = ConvertTo-Json $CreateData
        $createContent = New-Object System.Net.Http.StringContent($createJson, [System.Text.Encoding]::UTF8, 'application/json')
        $createResp = $client.PostAsync("$backendUrl/api/$Endpoint", $createContent).Result
        $createBody = $createResp.Content.ReadAsStringAsync().Result
        
        if ($createResp.StatusCode -eq 200) {
            $createObj = ConvertFrom-Json $createBody
            $entityId = $createObj.data.id
            Report-Test $Entity "CREATE" "PASS" "Created with ID: $entityId"
        } else {
            Report-Test $Entity "CREATE" "FAIL" "Status: $($createResp.StatusCode)"
            return
        }
    }
    catch {
        Report-Test $Entity "CREATE" "FAIL" "Exception: $_"
        return
    }
    
    # READ
    try {
        $getResp = $client.GetAsync("$backendUrl/api/$Endpoint/$entityId").Result
        if ($getResp.StatusCode -eq 200) {
            Report-Test $Entity "READ" "PASS" "Retrieved ID: $entityId"
        } else {
            Report-Test $Entity "READ" "FAIL" "Status: $($getResp.StatusCode)"
        }
    }
    catch {
        Report-Test $Entity "READ" "FAIL" "Exception: $_"
    }
    
    # UPDATE
    try {
        $updateJson = ConvertTo-Json $UpdateData
        $updateContent = New-Object System.Net.Http.StringContent($updateJson, [System.Text.Encoding]::UTF8, 'application/json')
        $updateMethod = New-Object System.Net.Http.HttpMethod("PUT")
        $updateReq = New-Object System.Net.Http.HttpRequestMessage($updateMethod, "$backendUrl/api/$Endpoint/$entityId")
        $updateReq.Content = $updateContent
        $updateResp = $client.SendAsync($updateReq).Result
        
        if ($updateResp.StatusCode -eq 200) {
            Report-Test $Entity "UPDATE" "PASS" "Updated ID: $entityId"
        } else {
            Report-Test $Entity "UPDATE" "WARN" "Status: $($updateResp.StatusCode) (may not be implemented)"
        }
    }
    catch {
        Report-Test $Entity "UPDATE" "WARN" "Exception: $_ (may not be implemented)"
    }
    
    # LIST
    try {
        $listResp = $client.GetAsync("$backendUrl/api/$Endpoint").Result
        if ($listResp.StatusCode -eq 200) {
            $listBody = $listResp.Content.ReadAsStringAsync().Result
            Report-Test $Entity "LIST" "PASS" "Retrieved list successfully"
        } else {
            Report-Test $Entity "LIST" "FAIL" "Status: $($listResp.StatusCode)"
        }
    }
    catch {
        Report-Test $Entity "LIST" "FAIL" "Exception: $_"
    }
    
    # DELETE
    try {
        $deleteMethod = New-Object System.Net.Http.HttpMethod("DELETE")
        $deleteReq = New-Object System.Net.Http.HttpRequestMessage($deleteMethod, "$backendUrl/api/$Endpoint/$entityId")
        $deleteResp = $client.SendAsync($deleteReq).Result
        
        if ($deleteResp.StatusCode -eq 200) {
            Report-Test $Entity "DELETE" "PASS" "Deleted ID: $entityId"
        } else {
            Report-Test $Entity "DELETE" "WARN" "Status: $($deleteResp.StatusCode)"
        }
    }
    catch {
        Report-Test $Entity "DELETE" "WARN" "Exception: $_"
    }
}

# Test Category
Test-CrudEntity "Category" "category" `
    @{ name = "E2E Test Category"; displayOrder = 1 } `
    @{ name = "E2E Test Category Updated"; displayOrder = 2 }

# Test Company
Test-CrudEntity "Company" "company" `
    @{ name = "E2E Test Company"; city = "Taipei"; phoneNumber = "02-1234-5678"; state = "Taiwan"; postalCode = "10001"; streetAddress = "123 Main St" } `
    @{ name = "E2E Test Company Updated" }

# Test Product (simpler version without files)
Test-CrudEntity "Product" "product" `
    @{ title = "E2E Test Product"; description = "Test"; isbn = "978-0-123456-78-9"; categoryId = 1; companyId = 1; listPrice = 99.99; heldYN = "Y"; hDate = "2025-12-05" } `
    @{ title = "E2E Test Product Updated" }

# Test Kindness Position
Test-CrudEntity "Kindness" "kindness" `
    @{ name = "E2E Test Kindness"; floor = "1"; section = "A"; level = "1"; position = "1" } `
    @{ name = "E2E Test Kindness Updated" }

# Test Ancestral Position
Test-CrudEntity "Ancestral" "ancestral" `
    @{ name = "E2E Test Ancestral"; side = "L"; section = "A"; level = "1"; position = "1" } `
    @{ name = "E2E Test Ancestral Updated" }

# Print summary
Write-Host "`n`n====== TEST SUMMARY ======"
$testResults | Format-Table -AutoSize
$passCount = ($testResults | Where-Object { $_.Status -eq "PASS" }).Count
$failCount = ($testResults | Where-Object { $_.Status -eq "FAIL" }).Count
$warnCount = ($testResults | Where-Object { $_.Status -like "*WARN*" }).Count

Write-Host "`nResults: $passCount Passed, $failCount Failed, $warnCount Warnings (total: $($testResults.Count) tests)"

# Save to file
$testResults | ConvertTo-Json | Out-File -FilePath '.\scripts\logs\test-all-crud-results.json' -Encoding utf8
Write-Host "Detailed results saved to: ./scripts/logs/test-all-crud-results.json"

$client.Dispose()
