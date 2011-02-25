[*] - new featuries for current release.

[*] Common UI refactoring. Skin support added.
[*] Performance improvements (most frequently requests are cached now).
[*] Bugfixes

Implemented features:
1. Common features
	1.1 WS Discovery to get available devices list
	1.2 Localization support
	1.3 Support for switch on/off modules
		- Object Tracker
		- Antishaker
	1.4 Support for Traditional China locale * Thanks mr. Kwlee for help with translation.
2. Identification and status
	2.1 Display device information:
		- Hardware ver.
		- Firmware ver.
		- Device ID
		- IP Address
		- MAC Adress
	2.2 Change and save device name.
	2.3 Change current date/time
3. Network settings
	3.1 Change DHCP On/Off
	3.2 Change device IP Address
	3.3 Change Subnet mask
	3.4 Change gateway address
	3.5 Change dns address
	3.6 Display MAC address
4. Maintenance
	4.1 Reboot button
	4.2 Display current firmware ver.
	4.3 Upgrade to new firmware
[*]	4.4 System backup/restore
5. Channels Support.
	5.2 Display name of each channel
	5.2 Display channel configutation pannel for each channel
	5.3 Display last event snapshot for every channel
[*]	5.4 Profile management
6. Live video
	6.1 Display real time video for selected channel
	6.2 Record live video to user default video folder	
7. Depth calibration
	7.1 Display real time video for selected channel
	7.2 Change Focal length in mm for selected channel
	7.3 Change Photosensor pixel size  for selected channel
	7.4 Change Matrix format  for selected channel
	7.5 Editor of the region, to set the preferred domain for object tracking
	7.6 Depth calibration markers
	7.7 Depth calibration on full screen mode
8. Video streaming
	8.1 Display real time video for selected channel
	8.2 Change resolution in pixels for selected channel
	8.3 Change frame rate in fps for selected channel
	8.4 Change encoder from supported list for selected channel
	8.5 Change bitrate in kbps for selected channel
	8.6 Change Encoding inteval for  selected channel
	8.7 Change channel name for  selected channel
9. Object tracker
	9.1 Display real time video for selected channel
	9.2 Change Contrast sensitivity for selected channel
	9.3 Change Object area minimul value
	9.4 Change Object area maximun value
	9.5 Change Speed maximum value in m/s
	9.6 Change Stabilization time in ms
	9.7 Change direction on region of interest
10. Events
	10.1 Display list of last 15 events for every channel
	10.2 Display snapshot for selected event
	10.3 Display list of common events wich are not related to the channels.	
11. Display Annotation
	11.1 Change settings for
		-Timestamp
		-Objects
		-Trajectories
		-Chanel name
		-Calibation results net
		-System info
12. Tampering detectors
	12.1 Scene to dark	
	12.2 Scene to bright
	12.3 Out of focus
	12.4 obstruction
	12.5 displacement
13. System logs
[*]	13.1 Systemlogs (MTOM support are added)
14. Dump (XML explorer)
	14.1 Display ONVIF dump from device
15. Antishaker
	15.1 Set Antishaker region
16. Time settings
	16.1 Display current device time and date
	16.2 Set device time and date with current computer time and date
	16.3 Set device TimeZone
	16.4 Set NTP Server
	16.5 Set manual time and date

