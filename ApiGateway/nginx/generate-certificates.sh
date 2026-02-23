#!/bin/sh

# Generate private key
openssl genrsa -out id-local.key 2048

# Generate certificate signing request
openssl req -new -key id-local.key -out id-local.csr -config id-local.conf/id-local.conf

# Generate self-signed certificate (valid for 365 days)
openssl x509 -req -days 365 -in id-local.csr -signkey id-local.key -out id-local.crt -extensions v3_ca -extfile id-local.conf/id-local.conf

# Create PKCS12 file (.pfx)
openssl pkcs12 -export -out id-local.pfx -inkey id-local.key -in id-local.crt -password pass:YourPassword123

# Clean up CSR file
rm id-local.csr

echo "Certificates generated successfully!"
echo "Files created:"
echo "  - id-local.key (private key)"
echo "  - id-local.crt (certificate)"
echo "  - id-local.pfx (PKCS12 format, password: YourPassword123)"

