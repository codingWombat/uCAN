uCAN.NetStandard
=====

Universal API for [Controller Area Network (CAN bus or CAN)](https://en.wikipedia.org/wiki/CAN_bus) adapters.

This is an incomplete port of some parts of the [original ucan release](https://github.com/codeskin/ucan).

## Project Objective

While [SocketCAN ](https://en.wikipedia.org/wiki/SocketCAN) is providing a great generic interface for using CAN adapters on Linux, no such implementation exists for Windows or macOS. This complicates the cross-platform development of CAN-based applications.

It is therefore the objective of this open-source project to provide an universal CAN API (uCAN) that covers all three major operating systems and offers a simple and clean DLL based API.

## Supported OS' and Hardware

uCAN.NetStandard currently supports the following hardware:
* Windows:
  * SL CAN compatible devices (such as [Lawicel](http://www.can232.com) and [Tiny-CAN](http://www.mhs-elektronik.de/))
* Linux:
  * SL CAN

## Software Dependencies

The portability of uCAN.NetStandard is based on the works of [Ryan Crosby (crozone)](https://github.com/crozone).

## IDE

While uCAN.NetStandard can be built from the command-line, it is recommended that [Visual Studio](https://visualstudio.microsoft.com/de/) be used to edit the source code.

## License

uCAN.NetStandard is released under the LGPL license. Please see the [LICENSE.txt](LICENSE.txt) file for more information.

## Contributing

In contrast to the original ucan release, this release was written by a real software engineer. ;)

We naturally also very much appreciate your bug reports, bug-fixes and feature contributions. Credit will always be given in the [Contributors.txt](Contributors.txt) file.
