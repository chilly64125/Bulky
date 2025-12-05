param()

# Ensure System.Net.Http is loaded
Add-Type -AssemblyName System.Net.Http

# Initialize HTTP client with cookie container
$handler = New-Object System.Net.Http.HttpClientHandler
$handler.UseCookies = $true
$handler.CookieContainer = New-Object System.Net.CookieContainer

$client = New-Object System.Net.Http.HttpClient($handler)

Write-Host "==== Step 1: Login as admin@chen ===="

try {
    $loginJson = '{"email":"admin@chen","password":"Admin1788@"}'
    $loginContent = New-Object System.Net.Http.StringContent($loginJson, [System.Text.Encoding]::UTF8, 'application/json')
    $loginResp = $client.PostAsync('http://localhost:5064/api/auth/login', $loginContent).Result
    
    $loginBody = $loginResp.Content.ReadAsStringAsync().Result
    $loginBody | Out-File -FilePath '.\scripts\logs\login_response.json' -Encoding utf8
    
    Write-Host "Login Response Status: $($loginResp.StatusCode)"
    Write-Host "Login Response Body:"
    Write-Host $loginBody
    
    if ($loginResp.StatusCode -ne 200) {
        Write-Host "Login failed. Exiting."
        exit 1
    }
}
catch {
    Write-Host "Login error: $_"
    exit 1
}

Write-Host "`n==== Step 2: Create multipart form with file ===="

try {
    $mp = New-Object System.Net.Http.MultipartFormDataContent
    
    # Add form fields
    $mp.Add([System.Net.Http.StringContent]::new('E2E Test Product', [System.Text.Encoding]::UTF8), 'title')
    $mp.Add([System.Net.Http.StringContent]::new('E2E product created by script', [System.Text.Encoding]::UTF8), 'description')
    $mp.Add([System.Net.Http.StringContent]::new('978-1-2345-6789-0', [System.Text.Encoding]::UTF8), 'isbn')
    $mp.Add([System.Net.Http.StringContent]::new('1', [System.Text.Encoding]::UTF8), 'categoryId')
    $mp.Add([System.Net.Http.StringContent]::new('1', [System.Text.Encoding]::UTF8), 'companyId')
    $mp.Add([System.Net.Http.StringContent]::new('2025-12-05', [System.Text.Encoding]::UTF8), 'hDate')
    $mp.Add([System.Net.Http.StringContent]::new('Y', [System.Text.Encoding]::UTF8), 'heldYN')
    $mp.Add([System.Net.Http.StringContent]::new('9.99', [System.Text.Encoding]::UTF8), 'listPrice')
    
    # Add file
    $filePath = 'D:\Git\VueChenClan\scripts\logs\test-image.png'
    if (-not (Test-Path $filePath)) {
        Write-Host "Test image not found: $filePath"
        exit 1
    }
    
    $fs = [System.IO.File]::OpenRead($filePath)
    $sc = New-Object System.Net.Http.StreamContent($fs)
    $sc.Headers.ContentType = [System.Net.Http.Headers.MediaTypeHeaderValue]::Parse('image/png')
    $mp.Add($sc, 'files', 'test-image.png')
    
    Write-Host "Posting to http://localhost:5064/api/product ..."
    
    $postResp = $client.PostAsync('http://localhost:5064/api/product', $mp).Result
    $postBody = $postResp.Content.ReadAsStringAsync().Result
    $postBody | Out-File -FilePath '.\scripts\logs\create_response.json' -Encoding utf8
    
    Write-Host "Create Response Status: $($postResp.StatusCode)"
    Write-Host "Create Response Body:"
    Write-Host $postBody
    
    $fs.Close()
}
catch {
    Write-Host "Upload error: $_"
    exit 1
}
finally {
    $client.Dispose()
}

Write-Host "`n==== E2E Upload Test Complete ===="
