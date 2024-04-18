//----------------------------------------------------------------------------
// name: r.ck ('r' is for "receiver")
// desc: OpenSoundControl (OSC) receiver example
// note: launch with s.ck (or another sender)
//----------------------------------------------------------------------------

// the patch
SinOsc osc1 => ADSR env1 => dac;

1000::ms => dur beat;

0.15 => osc1.gain;
(1::ms, beat / 4, 0, 1::ms) => env1.set;

spork ~ ReceiveOscMessage();

// infinite event loop
while( true )
{
    60 => Std.mtof => osc1.freq;
    1 => env1.keyOn;
    beat => now;
}

fun void ReceiveOscMessage()
{
    while (true)
    {
        // create our OSC receiver
        OscIn oin;
        // create our OSC message
        OscMsg msg;

        4560 => oin.port;
        // create an address in the receiver, expect an int and a float
        oin.addAddress( "/speed/gx" );
        // wait for event to arrive
        oin => now;

        // grab the next message from the queue. 
        while( oin.recv(msg) )
        {
            // fetch the first data element as int
            msg.getInt(0)::ms => beat;
            //<<< "New Beat X: ", beat >>>;
        }
    }
}