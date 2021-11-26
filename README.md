# UnityToMatlabUDP
Used to send positional and rotational data from Unity GameObjects to Matlab over UDP.

## Use

Simply add the `UDPSender.cs`-file to your Unity assets.

Create a `GameObject` called 'CommunicationManager' (or something else that clearly describes what it does).
In the `Inspector` for this `GameObject`, set the correct IP and ports and add `GameObject`s that you need the positional and rotational data of.

Now, press play in Unity and run `read_data.m` in Matlab.

If you are done sending data, press the Q-key on your keyboard (while the `Game`-window in Unity is selected). Matlab will now automatically stop saving data and close the connection.
You will then be prompted in Matlab to save the data.

### Important to know

Currently, only data for 2 `GameObject`s will be sent, regardless of how many you add to the list in the `Inspector`-panel (along with a timestamp).
Matlab will also look for only 2 `GameObject`s (which is 3 positions and 3 rotations each, plus the timestamp, so 13 values).

If you need to track more, edit the code (also in Matlab, line 11).
I didn't add a for-loop on purpose, as this would slow down the code. If that's not a problem, you can easily add it yourself.

### Saving the data to use in replay

I have added a script `save_replay.m` that allows you to easily save the data to a CSV-file that can then be replayed using my UnityDatReplay found here: https://github.com/pryxe023/UnityDataReplay

## Future updates

Nothing in the works right now.

### Known issues

* Currently; none!
