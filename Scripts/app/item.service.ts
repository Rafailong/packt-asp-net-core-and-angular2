import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/map';
import { Item } from "./item";

@Injectable()
export class ItemService {
  constructor(private http: Http) { }

  private baseUrl = "api/items/";

  getLatest(num?: Number): Promise<Item[]> {
    var url = this.baseUrl + "GetLatest/";
    if (num != null) {
      url += num;
    }
    return this.http.get(url)
      .toPromise()
      .then(response => response.json() as Item[])
      .catch(this.handleError)
  }

  getMostViewed(num?: Number): Promise<Item[]> {
    var url = this.baseUrl + "GetMostViewed/";
    if (num != null) {
      url += num;
    }
    return this.http.get(url)
      .toPromise()
      .then(response => response.json() as Item[])
      .catch(this.handleError)
  }

  getRandom(num?: Number): Promise<Item[]> {
    var url = this.baseUrl + "GetRandom/";
    if (num != null) {
      url += num;
    }
    return this.http.get(url)
      .toPromise()
      .then(response => response.json() as Item[])
      .catch(this.handleError)
  }

  get(id: Number): Promise<Item> {
    if (id == null) {
      throw new Error("id is required");
    }
    var url = this.baseUrl + id;
    return this.http.get(url)
      .toPromise()
      .then(response => response.json() as Item)
      .catch(this.handleError);
  }

  add(item: Item) {
    var url = this.baseUrl;
    return this.http.post(url, JSON.stringify(item), this.getRequestOptions())
      .map(r => r.json())
      .catch(this.handleError);
  }

  update(item: Item) {
    var url = this.baseUrl + item.Id;
    return this.http.put(url, JSON.stringify(item), this.getRequestOptions())
      .map(r => r.json())
      .catch(this.handleError);
  }

  delete(itemId: number) {
    var url = this.baseUrl + itemId;
    return this.http.delete(url)
      .catch(this.handleError);
  }

  private getRequestOptions() {
    return new RequestOptions({
      headers: new Headers({
        "Content-Type": "application/json"
      })
    });
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}