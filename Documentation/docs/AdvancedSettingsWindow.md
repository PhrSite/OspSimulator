# Advanced Settings Window
This window allows you to configure the advanced settings for the OspSimulator application.

## Network Settings

### Use Mutual Authentication
This setting applies to SIP over TLS. If this checkbox is checked, then the application shall offer its X.509 certificate when it attempts to connect to the remote host using SIP over TLS.

The default setting is un-checked.

### Use HTTPS
If checked, then the application listens on HTTPS for its HELD and additional data interfaces. If not checked, then the application will listen on HTTP.

The default setting is un-checked.
### Use IPv4 for HTTP(s)
If checked, then application uses IPv4 for the HTTP interfaces, else it uses IPv6 for the HTTP interfaces.

The default setting is checked.

### HTTP Port
This setting specifies the TCP port number to use for HTTP interfaces.

The default setting is 11000.

### HTTPS Port
This setting specifies the TCP port number to use for HTTPS interfaces.

The default setting is 11001.

### Local SIP Port
Specifies the SIP port for UDP and TCP.

The default setting is 5060.

### Local SIPS Port
Specifies the SIP port for Transport Layer Security (TLS).

The default setting is 5061.

### Media Ports
This application allocates media ports within separate ranges for each type of media. A port range is defined by a starting port number and a number of ports.

For audio, video and RTT, the application increments the port number by 2 each time a new port is required. This allows the odd port number to be used for RTCP. This means that the actual number of media connections is 1/2 of the specified number of ports.

For MSRP, the application increments the port number by one because MSRP does have the equivalent of the RTCP protocol.

When the last port number in a port range is used, the application wraps around the next allocated port number to the first port number in the range.

The following table shows the default media port settings.

| Media Type | Start Port | Number of Ports |
|------------|------------|-----------------|
| Audio      | 6000       | 100 |
| Video      | 7000       | 100 |
| RTT        | 8000       | 100 |
| MSRP       | 9000       | 100 |

The port ranges for audio, video and RTT may not overlap.

The port range for MSRP may overlap the port ranges for audio, video and RTT because MSRP uses TCP and audio, video and RTT use RTP over UDP.

## Certificate Settings
This application ships with a default X.509 certificate for use with Transport Layer Security (TLS). This default certificate is a self-signed certificate.

The certificate settings allow the user to either use the default self-signed certificate or to specify a user-provided X.509 certificate file.

### Use Default Certificate
If this checkbox is checked then the application will use the default X.509 certificate regardless of the Certificate File and Password settings.

If this checkbox is not checked then the application will use the X.509 certificate file specified in the Certificate File text box.

### Certificate File
Specifies the path to a user-provided X.509 PFX certificate file.

The certificate file path must be provided if the Use Default Certificate checkbox is un-checked.

The user-provided X.509 file must contain a private key and it must be password protected.

### Password
Specifies the password for the user-provided X.509 certificate file.

