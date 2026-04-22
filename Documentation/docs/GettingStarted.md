# Getting Started
Double-click on the shortcut called "OSP Simulator" on your computer's desktop to run the application.

Follow these steps to start a NG9-1-1 call from the application's [main window](MainWindow.md).

1. Enter a SIP URI into the ["To SIP URI"](MainWindow.md#ToSipUri) edit box. This field determines where to send the INVITE request.
1. Select a calling party number from the "From" combo box. The number selected in the From combo box determines what location and additional data is delivered with the call. See [Location and Additional Data](AdditionalDataAndLocation.md).
1. Select other configuration parameters such as how location and additional data will be delivered from the main form of the application.
1. Click on the Start Server button.
1. Click on the Start Call button. The label of this button will change to "Calling...".

You can cancel the call before it is answered by the called party by clicking on the "Calling..." button.

When the called party answers the call the application will display a window called [Current Call](CallForm.md) that displays information about the call.

The Current Call window will close when the called party ends the call.

You can end the call from the [Current Call](CallForm.md) window by clicking on the End Call button. The Current Call window will close automatically.



