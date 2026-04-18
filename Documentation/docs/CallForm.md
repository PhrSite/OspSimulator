# The Current Call Window

This window appears automatically when the called party answers the call.

This window shows various call status information and allows the user to perform the following actions.

1. View send and receive video if the call has video.
1. View received text message and send text messages if the call has RTT or MSRP media.
1. Add new media types to the call
1. End the call.

## Call Status Information
The Current Call Window displays the following call status information.

### To Display Field
This field displays the "To SIP URI" that the user entered in the main window.

### From Display Field
This field displays the calling party number entered in the From field of the main window.

### Media Available Display Field
This field shows a list of media that is currently available for the call. It will be blank if the call has no media.

This field is automatically updated to show the new media that the user has added to the call, provided that the called party accepted the offer of new media.

This field is automatically updated if the called party adds new media to the call.

### Audio Source Display Field
Indicates the source of audio sent to the called party. "Recording" indicates that the audio is from a pre-recorded file. "Microphone" indicates that the audio is captured from the computer's microphone.

## Add Media Button
This button displays a dialog box that allows the user to add new media to the current call.

Select the new media that you want to add and click on the OK button.

**Note:** Only one type of text media (RTT or MSRP) can be used for a call, you cannot select both.

## Text Messages
The Text Type display indicates the type of text media that is available for the call. This display will show "None" if no text media is available for the call, "RTT" if Real Time Text media is available for the call or "MSRP" if Message Session Relay Protocol media is available for the call.

If RTT or MSRP text media is available, the list box shows the text messages that have been sent and received during this call.

To send a new text message, type it in the New Message text box. If the text type is MSRP you can press the Enter keyboard key or click on the Send button to send the message. For RTT, each character is sent individually as you type them. Press the Enter key to clear the New Message text box in preparation for the next message.

If the text type is MSRP, a check box labeled "Use CPIM" will be visible. Check this checkbox to force the application to send MSRP text messages encapsulated within a CPIM message body. Uncheck this checkbox to send MSRP messages as plain text.

