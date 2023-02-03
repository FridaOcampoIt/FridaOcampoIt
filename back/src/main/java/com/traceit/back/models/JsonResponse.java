package com.traceit.back.models;

public class JsonResponse {
    private int status;
    private String message;
    private Object data;
    private String title;

    public static final int STATUS_OK = 200;
    public static final int STATUS_ERROR = 500;

    public int getStatus() {
        return status;
    }
    public void setStatus(int status) {
        this.status = status;
    }

    public String getMessage() {
        return message;
    }
    public void setMessage(String message) { this.message = message; }

    public Object getData() { return data; }
    public void setData(Object data) { this.data = data; }

    public String getTitle() { return title; }
    public void setTitle(String title) { this.title = title; }

    public JsonResponse() {}

    public JsonResponse(Object data, String message, int status) {
        this.status = status;
        this.message = message;
        this.data = data;
        this.title = null;
    }

    public JsonResponse(Object data) {
        this.status = STATUS_OK;
        this.message = "OK";
        this.data = data;
        this.title = null;
    }

    public JsonResponse(Object data, String message) {
        this.status = STATUS_OK;
        this.message = message;
        this.data = data;
        this.title = null;
    }

    public JsonResponse(Object data, String message, String title) {
        this.status = STATUS_OK;
        this.message = message;
        this.data = data;
        this.title = title;
    }

    public JsonResponse(Object data, String message, String title, int status) {
        this.status = status;
        this.message = message;
        this.data = data;
        this.title = title;
    }
}
