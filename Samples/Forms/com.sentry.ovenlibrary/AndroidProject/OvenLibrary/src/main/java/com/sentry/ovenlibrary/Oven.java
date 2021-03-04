package com.sentry.ovenlibrary;

public class Oven {
    static {
        System.loadLibrary("native-lib");
    }

    public native String Cook();
}
