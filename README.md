# BinaryPatcher

An extremely simple tool that applies plain text scripts of commands to a binary file in order to make changes to it.

Usage: `BinaryPatcher <bpatch script> <target file>`

## Commands
`g` - Go to a specific address. Requires one argument denoting the address.

`m` - Move cursor forwards. Requires one argument denoting the distance in bytes.

`b` - Move cursor backwards. Requires one argument denoting the distance in bytes.

`w` - Write, overwriting bytes at cursor position. Requires one or more arguments denoting the byte values to write, in order.

`l` - Loop, executing the previous command multiple times. Requires one argument denoting the number of times to run the single previous command. Loop commands cannot be looped.

All argument values are required to be hexadecimal numbers without any prefix.

Only one command per line is supported. Blank lines and comments are allowed, with suggested comment prefixes of `//`, `--`, or `#`. Multi-line comments require a prefix on all lines.

## Example script
```
g 1D54C       # Go to offset 0x1D54C in the target file
w 01 02 FE FF # Overwrite four bytes at that position (moving cursor to 0x1D550)
m 4           # Move forwards four bytes (to 0x1D554)
w 00          # Overwrite one byte at that position
l 8           # Execute that one-byte write 8 times
```