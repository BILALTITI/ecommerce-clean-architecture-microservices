# PowerShell script to generate SSL certificates for nginx

# Check if OpenSSL is available
$opensslAvailable = Get-Command openssl -ErrorAction SilentlyContinue

if (-not $opensslAvailable) {
    Write-Host "OpenSSL is not installed or not in PATH." -ForegroundColor Red
    Write-Host "Please install OpenSSL or use Docker to generate certificates." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Using Docker to generate certificates..." -ForegroundColor Cyan
    
    # Use Docker to run OpenSSL
    docker run --rm -v "${PWD}":/work -w /work alpine/openssl genrsa -out id-local.key 2048
    
    docker run --rm -v "${PWD}":/work -w /work alpine/openssl req -new -key id-local.key -out id-local.csr -config id-local.conf/id-local.conf
    
    docker run --rm -v "${PWD}":/work -w /work alpine/openssl x509 -req -days 365 -in id-local.csr -signkey id-local.key -out id-local.crt -extensions v3_ca -extfile id-local.conf/id-local.conf
    
    docker run --rm -v "${PWD}":/work -w /work alpine/openssl pkcs12 -export -out id-local.pfx -inkey id-local.key -in id-local.crt -password pass:YourPassword123
    
    # Clean up CSR
    Remove-Item id-local.csr -ErrorAction SilentlyContinue
    
} else {
    Write-Host "Using local OpenSSL installation..." -ForegroundColor Cyan
    
    # Generate private key
    openssl genrsa -out id-local.key 2048
    
    # Generate certificate signing request
    openssl req -new -key id-local.key -out id-local.csr -config id-local.conf/id-local.conf
    
    # Generate self-signed certificate
    openssl x509 -req -days 365 -in id-local.csr -signkey id-local.key -out id-local.crt -extensions v3_ca -extfile id-local.conf/id-local.conf
    
    # Create PKCS12 file
    openssl pkcs12 -export -out id-local.pfx -inkey id-local.key -in id-local.crt -password pass:YourPassword123
    
    # Clean up CSR
    Remove-Item id-local.csr -ErrorAction SilentlyContinue
}

Write-Host ""
Write-Host "Certificates generated successfully!" -ForegroundColor Green
Write-Host "Files created:" -ForegroundColor Green
Write-Host "  - id-local.key (private key)"
Write-Host "  - id-local.crt (certificate)"
Write-Host "  - id-local.pfx (PKCS12 format, password: YourPassword123)"

