# LAdotNET - LOST ARK KR Emulator

LAdotNET is a project I started back in November 2019. I haven't had the time nor drive to work on it anymore.

## Screenshots
![image](https://cloud.githubusercontent.com/assets/711743/25648417/57cd2c0c-2fe9-11e7-8753-b60ea2656faf.png)

## Client

Supports KR client version 1.6.4.1
You will be able to find it around the net.

## Information

Set your config files up like so:
```python
loginserver/
 - config/
  - loginserver_config.yml
  - network_config.yml
  
gameserver/
 - config/
  - network_config.yml
```

Worldserver does not have any config.

You are able to login, create a character and walk around in the first zone.
Most of the packet data is hardcoded as I was just in the process of getting everything set up but there is a solid base here.

Packet data structures change literally every update, they swap shit around. You will be able to see what I'm talking about in some of the packet files.

I have also included a sniffer that might work if updated to the new client? I doubt they changed their "encryption".

The emulator supports KR client 1.6.4.1 which I will have uploaded by the time this goes public.

This was purely for learning purposes only, I love messing around with networking, just wanted to throw it up on Github instead of throwing it away.

## License
[GNU](https://www.gnu.org/licenses/)
