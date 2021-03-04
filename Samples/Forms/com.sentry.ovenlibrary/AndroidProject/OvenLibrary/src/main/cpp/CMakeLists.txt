cmake_minimum_required(VERSION 3.10.2)

project("OvenLibrary")

add_library( 
             native-lib
             SHARED
             native-lib.cpp )


find_library( # Sets the name of the path variable.
              log-lib
              log )

target_link_libraries( # Specifies the target library.
                       native-lib
                       ${log-lib} )