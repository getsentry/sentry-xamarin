#include <stdio.h>
#include <jni.h>
#include <string>

extern "C" JNIEXPORT jstring JNICALL Java_com_sentry_ovenlibrary_Oven_Cook( JNIEnv * env, jobject obj) {
    std::string hello = "Hello from C++";
    char *ptr = 0;
    *ptr += 1;
    return env->NewStringUTF(hello.c_str());
}