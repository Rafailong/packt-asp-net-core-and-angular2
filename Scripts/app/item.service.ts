import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
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

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}