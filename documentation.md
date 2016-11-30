## Introduction

There are generally five ways an application might want to use Bluetooth:  

**1\. Make a direct data connection**  
Where the program connects directly to a Bluetooth RFCOMM service, and sends and receives the raw data for that connection. See section [General Bluetooth Data Connections](#General) below. The **BluetoothClient** provides the **Stream** to read and write on -- there is no need to use virtual COM ports and the System.IO.Ports.SerialPort class. The server side function can also be provided. Our recommendation is that only if you are setting up the connection for a separate program that uses COM ports do you need to use virtual COM ports, see 1b.  

**1b. Virtual Serial Port**  
If however you have a separate third-party application that needs to use a virtual COM port then it is not quite so simple and direct. See [Bluetooth Serial Ports](http://32feet.codeplex.com/wikipage?title=Bluetooth%20Serial%20Ports&referringTitle=Documentation).  

Note that RFCOMM/SPP only allows one connection from a remote device to each service. So do not use BluetoothClient and a virtual Serial port to connect to the same service at the same time; the second one will fail to connect.  

**2\. Do an OBEX transfer**  
Where the program is an OBEX client and connects to a server, and sends (PUTs) or GETs a file/object. The server side function can also be provided. See section [OBEX](#OBEX) below.  

**3\. Have the Bluetooth stack and/or the OS connect to and use a remote service**  
Common services for this case are where the service is Headset/Handsfree/A2DP, or networking for instance. Here we do not want the program to connect directly to those services, as we wouldn’t know what to do with the raw bytes, but instead want the OS to send audio to the headset, or form a network connection with an access-point or similar. See section [Connecting to Bluetooth Services](#Connecting) below.  

**4\. Make a direct data connection using the L2CAP protocol**  
Where the program connects directly to a Bluetooth L2CAP service, and sends and receives the raw data for that connection. We have pre-release support for this on some platforms see section [Bluetooth L2CAP](#Bluetooth) below,  

**5\. Check whether a particular device is in range**  
The best way to do this is to attempt to open a connection to a service on the device and see if it succeeds. One service that is always present is SDP so do a fake SDP query, see section [Testing if a device is in range](#Testing) below. If a device comes into range but neither it nor the local device attempt a connection nor are running discovery, then neither will know they are in range.  

As noted there, we have support for most of those currently. See [http://www.alanjmcf.me.uk/comms/bluetooth/Bluetooth Profiles and 32feet.NET.html](http://www.alanjmcf.me.uk/comms/bluetooth/Bluetooth Profiles and 32feet.NET.html) for information on what services use which method.  

For device discovery see the section under [General Bluetooth Data Connections](#General) below.  

## Contents

*   [Supported Hardware and Software](http://32feet.codeplex.com/wikipage?title=Supported%20Hardware%20and%20Software&referringTitle=Documentation)
    *   [Microsoft](http://32feet.codeplex.com/wikipage?title=Microsoft&referringTitle=Documentation)
        *   [Windows Store Apps](http://32feet.codeplex.com/wikipage?title=Windows%20Store%20Apps&referringTitle=Documentation)
        *   [Windows Phone 8](http://32feet.codeplex.com/wikipage?title=Windows%20Phone%208&referringTitle=Documentation)
    *   [Broadcom/Widcomm](http://32feet.codeplex.com/wikipage?title=Broadcom&referringTitle=Documentation)
    *   [BlueSoleil](http://32feet.codeplex.com/wikipage?title=BlueSoleil&referringTitle=Documentation)
    *   [BlueZ on Linux](http://32feet.codeplex.com/wikipage?title=BlueZ%20on%20Linux&referringTitle=Documentation)
    *   [Stonestreet One Bluetopia](http://32feet.codeplex.com/wikipage?title=Stonestreet%20One%20Bluetopia&referringTitle=Documentation)
    *   [Android](http://32feet.codeplex.com/wikipage?title=Android&referringTitle=Documentation) (alpha)
    *   [Feature support table](http://32feet.codeplex.com/wikipage?title=Feature%20support%20table&referringTitle=Documentation)
    *   [Stack Identification](http://32feet.codeplex.com/wikipage?title=Stack%20Identification&referringTitle=Documentation) (preliminary)
    *   [Switching any dongle to the Microsoft stack](http://32feet.codeplex.com/wikipage?title=Switching%20any%20dongle%20to%20the%20Microsoft%20stack&referringTitle=Documentation)
*   [Multi-stack Support](http://32feet.codeplex.com/wikipage?title=Multi-stack%20Support&referringTitle=Documentation)
*   [Referencing the library](http://32feet.codeplex.com/wikipage?title=Referencing%20the%20library&referringTitle=Documentation)
*   [Visual Basic Samples](http://32feet.codeplex.com/wikipage?title=Visual%20Basic%20Samples&referringTitle=Documentation)
*   [Sample programs](http://32feet.codeplex.com/wikipage?title=Sample%20programs&referringTitle=Documentation)
*   [OBEX](http://32feet.codeplex.com/wikipage?title=OBEX&referringTitle=Documentation)<a name="OBEX"></a>
    *   [Behaviour From Servers](http://32feet.codeplex.com/wikipage?title=Behaviour%20From%20Servers&referringTitle=Documentation)
    *   [Server-side](http://32feet.codeplex.com/wikipage?title=Server-side&referringTitle=Documentation)
        *   [One Active Server](http://32feet.codeplex.com/wikipage?title=One%20Active%20Server&referringTitle=Documentation)
    *   [Brecham.Obex](http://32feet.codeplex.com/wikipage?title=Brecham.Obex&referringTitle=Documentation)
*   [General Bluetooth Data Connections (RFCOMM)](http://32feet.codeplex.com/wikipage?title=General%20Bluetooth%20Data%20Connections&referringTitle=Documentation)<a name="General"></a>
    *   [Discovery](http://32feet.codeplex.com/wikipage?title=Discovery&referringTitle=Documentation)
        *   [Asynchronous Device Discovery](http://32feet.codeplex.com/wikipage?title=Asynchronous%20Device%20Discovery&referringTitle=Documentation)
    *   [DeviceName and Discovery](http://32feet.codeplex.com/wikipage?title=DeviceName%20and%20Discovery&referringTitle=Documentation)
    *   [Bluetooth Server-side (RFCOMM)](http://32feet.codeplex.com/wikipage?title=Bluetooth%20Server-side&referringTitle=Documentation)
    *   [Errors](http://32feet.codeplex.com/wikipage?title=Errors&referringTitle=Documentation)
    *   [Stream.Read](http://32feet.codeplex.com/wikipage?title=Stream.Read&referringTitle=Documentation)
    *   [Connected Property](http://32feet.codeplex.com/wikipage?title=Connected%20Property&referringTitle=Documentation)
    *   [RFCOMM Control Information](http://32feet.codeplex.com/wikipage?title=RFCOMM%20Control%20Information&referringTitle=Documentation)
*   [Connecting to Bluetooth Services](http://32feet.codeplex.com/wikipage?title=Connecting%20to%20Bluetooth%20Services&referringTitle=Documentation)<a name="Connecting"></a>
*   [General IrDA Connections](http://32feet.codeplex.com/wikipage?title=General%20IrDA%20Connections&referringTitle=Documentation)
*   [Bluetooth Settings](http://32feet.codeplex.com/wikipage?title=Bluetooth%20Settings&referringTitle=Documentation)
    *   [Peer Device Information](http://32feet.codeplex.com/wikipage?title=Peer%20Device%20Information&referringTitle=Documentation)
    *   [Local Radio Information](http://32feet.codeplex.com/wikipage?title=Local%20Radio%20Information&referringTitle=Documentation)
*   [Bluetooth Serial Ports](http://32feet.codeplex.com/wikipage?title=Bluetooth%20Serial%20Ports&referringTitle=Documentation)
    *   [Getting Virtual COM Port Names](http://32feet.codeplex.com/wikipage?title=Getting%20Virtual%20COM%20Port%20Names&referringTitle=Documentation)
*   [Bluetooth Security](http://32feet.codeplex.com/wikipage?title=Bluetooth%20Security&referringTitle=Documentation)
    *   [BluetoothWin32Authentication](http://32feet.codeplex.com/wikipage?title=BluetoothWin32Authentication&referringTitle=Documentation)
*   [Bluetooth SDP](http://32feet.codeplex.com/wikipage?title=Bluetooth%20SDP&referringTitle=Documentation)
    *   [Creating Records](http://32feet.codeplex.com/wikipage?title=Creating%20Records&referringTitle=Documentation)
    *   [Connect by Service Name](http://32feet.codeplex.com/wikipage?title=Connect%20by%20Service%20Name&referringTitle=Documentation)
    *   [Manual Record Creation](http://32feet.codeplex.com/wikipage?title=Manual%20Record%20Creation&referringTitle=Documentation)
*   [Testing if a device is in range](http://32feet.codeplex.com/wikipage?title=Testing%20if%20a%20device%20is%20in%20range&referringTitle=Documentation)<a name="Testing"></a>
*   [Bluetooth L2CAP](http://32feet.codeplex.com/wikipage?title=Bluetooth%20L2CAP&referringTitle=Documentation)<a name="Bluetooth"></a>
*   [BluetoothWin32Events](http://32feet.codeplex.com/wikipage?title=BluetoothWin32Events&referringTitle=Documentation)
*   [32feet.NET and shells](http://32feet.codeplex.com/wikipage?title=32feet.NET%20and%20shells&referringTitle=Documentation), including PowerShell