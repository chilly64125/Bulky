param()

# Test configuration
$backendUrl = 'http://localhost:5064'

# Login first
Add-Type -AssemblyName System.Net.Http
$handler = New-Object System.Net.Http.HttpClientHandler
$handler.UseCookies = $true
$handler.CookieContainer = New-Object System.Net.CookieContainer
$client = New-Object System.Net.Http.HttpClient($handler)

$loginJson = '{"email":"admin@chen","password":"Admin1788@"}'
$loginContent = New-Object System.Net.Http.StringContent($loginJson, [System.Text.Encoding]::UTF8, 'application/json')
$loginResp = $client.PostAsync("$backendUrl/api/auth/login", $loginContent).Result
Write-Host "Login: $($loginResp.StatusCode)"

# Test each API endpoint with detailed output
function Test-Endpoint {
    param([string]$Endpoint, [hashtable]$Data)
    
    Write-Host "`n========== Testing $Endpoint =========="
    $json = ConvertTo-Json $Data
    Write-Host "Request Body: $json"
    
    $content = New-Object System.Net.Http.StringContent($json, [System.Text.Encoding]::UTF8, 'application/json')
    $resp = $client.PostAsync("$backendUrl/api/$Endpoint", $content).Result
    $body = $resp.Content.ReadAsStringAsync().Result
    
    Write-Host "Response Status: $($resp.StatusCode)"
    Write-Host "Response Body: $body"
}

# Test Category creation (simple)
Test-Endpoint "category" @{ name = "Test Cat 1"; displayOrder = 1 }

# Test Company creation
Test-Endpoint "company" @{ 
    name = "Test Co"; 
    city = "Taipei"; 
    phoneNumber = "02-1234-5678"; 
    state = "Taiwan"; 
    postalCode = "10001"; 
    streetAddress = "123 Main" 
}

# Test Product creation (with required fields)
Test-Endpoint "product" @{
    title = "Test Prod"
    description = "Desc"
    isbn = "978-0-123456-78-9"
    categoryId = 1
    companyId = 1
    listPrice = 99.99
    heldYN = "Y"
    hDate = "2025-12-05"
}

# Test Kindness
Test-Endpoint "kindness" @{
    name = "Test Kindness"
    floor = "1"
    section = "A"
    level = "1"
    position = "1"
}

# Test Ancestral
Test-Endpoint "ancestral" @{
    name = "Test Ancestral"
    side = "L"
    section = "A"
    level = "1"
    position = "1"
}

$client.Dispose()
