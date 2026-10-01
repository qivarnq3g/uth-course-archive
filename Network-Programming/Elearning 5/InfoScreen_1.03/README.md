# BT5.1 - InfoScreen Enhanced (Base/Reference)
**Location:** `Elearning 5\InfoScreen_1.03\`

## Changes Made
- ✅ Fixed Timer1 interval: 2000ms → 10000ms (10 seconds)
- ✅ BalloonTipText display with timestamp already implemented
- ✅ Message protocol with # delimiter (STATUS, SENDALL, SENDUSER)
- ✅ ArrayList-based user list management
- ✅ Auto-clear userList every 10 seconds (Timer2)

## How to Test
1. Run the application, click **Start** to listen on port 9099
2. Open another instance or client
3. Enter username and message
4. Verify STATUS message sent every 10 seconds
5. Send SENDALL/SENDUSER messages and verify BalloonTip displays

## Features
- Broadcast messaging to all connected clients
- Unicast messaging to specific user
- Real-time user list updates
- System tray notifications (BalloonTip)
- Graceful exit handling
