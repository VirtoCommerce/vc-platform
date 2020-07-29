# OAuth2 using Json Web Tokens

In our platform we use JWT tokens mechanism by using OAuth2 protocol. Password, RefreshToken and ClientCredentials flow are supported. Tokens issued by platform are signed with the private key. For tokens validations it is possibile to use public certificate or Authority URL. Paths to certificates and Authority URL are specified in section **Auth** in **appsetting.json** file. 

## Example of creation self-signed certificates for signature and validation of tokens using OpenSSL

### Generation of private key
`openssl.exe genpkey -outform PEM -algorithm RSA -pkeyopt rsa_keygen_bits:4096 -pass file:certpass.txt -des3 -out virtocommerce.key`
- In **certpass.txt** file you should specify a password for the private key (the system will not work if the key file is not protected with password)

### Generation of certificate
`openssl.exe req -x509 -nodes -days 3650 -key virtocommerce.key -config certconfig.txt -extensions req_ext -passin file:certpass.txt -out virtocommerce.crt`

### Example of certconfig.txt file
    [ req ]
    default_md = sha256
    prompt = no
    req_extensions = req_ext
    distinguished_name = req_distinguished_name
    [ req_distinguished_name ]
    commonName = virtocommerce.com
    countryName = RU
    stateOrProvinceName = Kaliningrad
    organizationName = Virtocommerce
    [ req_ext ]
    subjectKeyIdentifier = hash
    authorityKeyIdentifier = keyid:always,issuer
    keyUsage=critical,digitalSignature,keyEncipherment
    extendedKeyUsage=critical,serverAuth,clientAuth
    subjectAltName = @alt_names
    [ alt_names ]
    DNS.0 = virtocommerce.com
###  Creation of pfx container for private key and certificate
`openssl.exe pkcs12 -export -out virtocommerce.pfx -inkey virtocommerce.key -in virtocommerce.crt`
- The system does not accept private keys smaller than 2048 Bits

# OAuth2 Authorization using Client credential flow
## How to add new client
In order to authorize client applications (for example, Storefront), it is possible to use the **Client credential flow** mechanism of  OAuth2 protocol.
- Go to blade **OAuth applications** in **Security** menu and create new application using **Add** button. This will automatically generate **Client Id** and **Client secret**, which should be saved, as system will not allow you to view already saved **Client secret**. Once all fields are filled in you should click **Ok** and a new OAuth2 client will be created.<br>
**Note:** You can change **Client Id** and **Client secret** only in the process of creation of new application (you will not be able to change them in future). You can also specify **Display Name** for more information.

- Then the client application will be able to authorize requests to the Api using the previously created **Client Id** and **Client secret**.
For example, in Storefront, all you need to do is specify the **Client Id**, **Client secret** created earlier, and also specify the **authorization server** in the **Endpoint** section of the **appsettings.json** file.
