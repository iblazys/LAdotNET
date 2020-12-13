# LAdotNET - LOSTARK KR Emulator

This is an emulator for Lost Ark that I started to develop back in November 2019. I got sidetracked with other things in life and was not able to continue working on it.

You are able to login, create a character and walk around in the first zone.
Most of the packet data is hardcoded as I was just in the process of getting everything set up but there is a solid base here.

Packet data structures change literally every update, they swap shit around. You will be able to see what I'm talking about in some of the packet files.

I have also included a sniffer that might work if updated to the new client? I doubt they changed their "encryption".

The emulator supports KR client 1.6.4.1 which I will have uploaded by the time this goes public.

This was purely for learning purposes only, I love messing around with networking.

Make sure x64lahook.dll and x64lahook.ini are in the same folder (LOSTARK base folder)

Inject x64lahook.dll and you should be able to connect to the server.
