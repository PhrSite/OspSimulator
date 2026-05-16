# Version History

## v1.1.0 - 15 May 2026
| Issue No. | Change Type | Description |
|--------|--------|-------|
| NA     | Fix    | When building an answer MediaDescription for MSRP, was producing "m=message 9006 TCP/MSRP 0" instead of "m=message 9006 TCP/MSRP *" |
| NA     | Fix    | The video format list was not properly displaying the last selected video format. |
| NA     | Fix    | The Timestamp field in the RTP packets for video media was not being incremented correcty if the video frame rate for video capture was not 30 frames/second. |
| NA     | Fix    | Was not setting frame rate to the configured value for the video capture device. |
| Addition | Added the Call-Info headers for the NG9-1-1 emergency-CallId and the emergency-IncidentId |

## v1.0.0 - 25 Apr 2026
| Issue No. | Change Type | Description |
|--------|--------|-------|
| NA       |  New      | Initial version |




