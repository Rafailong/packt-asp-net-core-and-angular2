import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
@Injectable()
export class AuthHttp {
  http: Http = null;
  authKey = "auth";

  constructor(http: Http) {
    this.http = http;
  }

  get(url: string, opts = {}) {
    this.configureAuth(opts);
    return this.http.get(url, opts);
  }

  post(url: string, data: any, opts = {}) {
    this.configureAuth(opts);
    return this.http.post(url, data, opts);
  }

  put(url: string, data: any, opts = {}) {
    this.configureAuth(opts);
    return this.http.put(url, data, opts);
  }

  delete(url: string, opts = {}) {
    this.configureAuth(opts);
    return this.http.delete(url, opts);
  }

  configureAuth(opts: any) {
    const i = localStorage.getItem(this.authKey);
    if (i != null) {
      const auth = JSON.parse(i);
      console.log(auth);
      if (auth.access_token != null) {
        if (opts.headers == null) {
          opts.headers = new Headers();
        }
        opts.headers.set("Authorization", `Bearer ${auth.access_token}`);
      }
    }
  }
}