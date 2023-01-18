<img src="Qbe.png" style="align-items: center" width="150"/>

# Qbe
This is a minimalistic language, designed specifically to mess around with the pointer. 
The language has support for a higher-level code, using the built-in`func` keyword to make functions.

### Official Modules
HLL - HLL or High-Level Library is a library that adds functions that would usually be already implemented (like `for` and `while`) to an OOB Langage like C#, Java. .


### Data Types
__Do Note:__ A name of an (official) module will be in brackets `()` to indicate in what module that thing can be found.

| `DataType` | `Handled as DataType`   |
|------------|-------------------------|
| `bin`      | `int` <br>`float` (HLL) |
| `bit`      | `bool`                  | 
| `ascii`    | `string` <br> `char`    |

### Reference Operators
| Handle    | Description                   |
|-----------|-------------------------------|
| `@<reg>`  | Address of the given register |
| `%<reg>`  | Value of the given register   |

### Instructions

| Command and `<arguments>` | Description                                                             |
|---------------------------|-------------------------------------------------------------------------|
| `setptrpos <int>`         | Set pointer position to the given value                                 |
| `getptrpos`               | Get pointer position (returns bin)                                      |
| `up`                      | Move up by 1                                                            |
| `up <bin>`                | Move up by the given amount                                             |
| `down`                    | Move down by 1                                                          |
| `down <bin>`              | Move down by the given amount                                           |
| `getaddrpos <reg>`        | Get address of the specified register                                   |
| `setaddrpos <reg> <bin>`  | Set address position of the specified register to the specified address | 
| `incr`                    | Increment current address by 1                                          |
| `incr <int>`              | Increment current address by the given amount                           |                                            
| `decr`                    | Decrement current address by 1                                          |
| `decr <int>`              | Decrement current address by the given amount                           |     
| `func <string> (any)`     | Function with name as string with one argument                          |


<br>

### Support
Due to the fact that this is a prototype, this project is written in C#, developedd with .Net 6 (soon to be updated with .Net 7 on November 2022), All of the Desktop Operating Systems (and other OSs that have .Net Support) are supported automatically (compilation script coming soon).
- [x] Linux
- [x] Windows
- [x] MacOS


<ins> Do note that this is a prototype of the C/C++ version of this project/repo. The actual version will come out at its initial release </ins>
# Qbe
# Qbe
