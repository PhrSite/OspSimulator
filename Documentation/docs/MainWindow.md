# The Main Application Window
The main application window provides various configuration settings and controls that allow you to start a call.

# Call Address Settings

## To SIP URI
This text field specifies the address of who to call in the form of a SIP URI.

The following table show some examples of valid SIP URIs.

| SIP URI | Description |
|---------|-------------|
| sip:911@192.168.64:5060 | Send an INVITE request to IPv4 address = 192.168.1.64 on port 5060 using UDP |
| sip:911@192.168.64:5060;transport=tcp | Send an INVITE request to IPv4 address = 192.168.1.64 on port 5060 using TCP |
| sip:911@[2600:1700:7A40:4740::525];transport=tcp | Send an INVITE to IPv6 address = 2600:1700:7A40:4740::525 on port 5060 using TCP |
| sips:911@psapsimulator.net;transport=tcp | Perform a DNS query for psapsimulator.net. Send an INVITE to the resolved IP address on port 5061 using SIPS (SIP over TLS) |

The host portion of the SIP URI can either be host name/domain or an IP endpoint consisting of an IP address and a port number.

If the host portion of the SIP URI is a host name then the application will lookup the host name via a DNS request to resolve it to an IP address. If the computer has an IPv4 address then the application will perform a A record DNS query. If the computer has an IPv6 address then the application will perform a AAAA record DNS query.

If the application finds both an IPv4 and an IPv6 address for the host name then it will use the Prefer IPv6 setting to determine which IP address to use. If Prefer IPv6 is checked then the application will use the IPv6 address, else it will use the IPv4 address.

If the host portion is an IP endpoint then the application will not perform a DNS query.

The port parameter of the SIP URI is optional. If a port is not specified then the application will use 5060 for SIP and 5061 for SIPS (SIP over TLS).

## Use urn:service:sos
If this checkbox is checked the the request URI for the outgoing INVITE request will be set to "urn:service:sos". If this checkbox is not checked then the request URI will be set to the To SIP URI.

## From

This combo box allows you to select the calling party number. It contains a list of predefined numbers.

If you select a predefined number, the application will also send location data and additional data that has been configured for that number depending upon the location and additional data configuration parameters.

You can also type in anything into the text field of this combo box. If the entry does not correspond to a predefined calling party number then location and additional data will not be available for the call.

## Use tel URI

If this checkbox is checked then the application will put a tel URI in the From header of the outgoing SIP INVITE request. For for example:

```
From: "6306820001" <tel:+16306820001>;tag=lfh74maad2
```

If this checkbox is not checked then the application will put a SIP URI in the From header as follows.

```
From: "6306820001" <sip:6306820001@192.168.1.76:5060;transport=tcp>;tag=cx9sgz6f5u
```

# Local IP Addresses
These settings specify which network types to use (IPv4 and/or IPv6) and allow you to specify which address to use for each network type.

Both IPv4 and IPv6 may be enabled.

The local address that is used for a call depends upon these settings and the network type specified in the "To SIP URI" setting.

The Prefer IPv6 checkbox specifies (if checked) to use IPv6 if IPv6 is enabled and the the "To SIP URI" resolves to both an IPv4 address and an IPv6 address.

# Offer Media Settings
These settings specify which media types the application will offer in the outgoing INVITE request.

It is possible to offer no media with the initial INVITE request for the call by un-checking all media types. The user can then add media once the call is answered if the called party allows a call with no media.

The application will only allow one type of text media (RTT or MSRP) to be selected for a call.

If the "Use Message/CPIM" checkbox is checked then the application will always send MSRP text messages using an MSRP content type of "message/CPIM". Otherwise the application will always use a MSRP content type of "text/plain".

# Media Encryption Settings
These settings specify type to of encryption to use for RTP media (audio, video and RTT) and MSRP media.

Media encryption is independent of the encryption used for SIP.

# Codec Settings
The codec settings specify which media codecs are offered in the initial INVITE request or in a re-INVITE request to add a new media type to the call.

# Audio Settings
The audio settings allow you to specify which audio device to use and the source of audio that will be sent to the called party.

The audio device can either be the computer's speakers and microphone or a headset with a microphone.

You can select the audio device to use using the Audio Device combo box in the event that there are multiple audio devices connected to your computer.

If the Use Recorded Audio checkbox is checked, then audio from a pre-recorded audio file will be sent to the called party instead of audio from the microphone.

**Note:** You cannot change the source of the audio once a call has been started.

This application provides a default audio recording. The default audio recording will be used if the Use Default Audio Recording checkbox is checked. The default audio recording repeats a message that says:
> This is a non-emergency test recording.

The default recording repeats every 5 seconds.

The user can use their own pre-recorded audio file by checking the Use Recorded Audio checkbox and un-checking the Use Default Audio Recording checkbox.

The file location of the user's pre-recorded audio file can be specified in the Recording File textbox.

The user-provided audio file must be a Windows WAVE file with the following characteristics.

- Sample Format: 16-bit linear PCM
- Sample Rate: 8000 or 16000 samples/second

# Video Settings
The video settings allow the user to select which video device (camera) to use for capturing video and the format of the video to be captured.

The supported video capture formats (Sub-Types) are NV12, YUY2 and RGB.

# Location and Additional Data

## Location Settings
This application supports the following methods of delivering caller location data for a call.

- Location By-Value
- Location By-Reference
- Location By SIP Presence Event

Any combination of the above delivery may be selected. If no location delivery methods are selected then location data will not be available for the call.

## Additional Data Settings
The application supports the following methods for delivering additional data for a call.

- Additional Data By-Value
- Additional Data By-Reference

The user may select one, both or none of the additional data delivery methods.

# Button Controls

## Start Server
Starts the application's HTTP server for location and additional data.

When the server starts, the label of this button changes to "Stop Server". If you click on the button when the label indicates "Stop Server" then the application stops its HTTP server.

## Advanced Settings
This button displays the [Advanced Settings Window](AdvancedSettingsWindow.md).

## Start Call
Start a new call. The button label changes to "Calling...".

You can cancel the call request by clicking on it when it indicates "Calling...".

The application will automatically display the [Current Call Window](CallForm.md) when the call is answered by the called party.

## Help
Displays the on-line help topic for this application.

## Close
Closes the application.


