#include <stdio.h>
#include <jni.h>
#include <string>

extern "C" JNIEXPORT jstring JNICALL Java_com_sentry_ovenlibrary_Oven_Cook( JNIEnv * env, jobject obj) {
    std::string hello = "Hello from C++";
    int *memory = (int*)-10;
    memory = (int*)realloc(memory, 65535 );
    for(int i = 0; i < 65535; i++) {
        memory[i] = -i;
    }
    free((int*)-10);
    free(memory);
    return env->NewStringUTF(hello.c_str());
}