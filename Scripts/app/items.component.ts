import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { Item } from "./item";
import { ItemService } from "./item.service";

@Component({
  selector: "item-list",
  template: `
    <h2>{{title}}</h2>
    <ul class="items">
      <li *ngFor="let item of items"
        [class.selected]="item === selectedItem"
        (click)="onSelect(item)">
          <span>{{item.Title}}</span>
        </li>
    </ul>
  `,
  styles: [`
    ul.items li {
      cursor: pointer;
    }
    ul.items li.selected {
      background-color: #cccccc;
    }
  `]
})
export class ItemListComponent implements OnInit {
  @Input() class: string;
  title: string;

  selectedItem: Item;
  items: Item[];
  errorMessage: string;

  constructor(
    private itemService: ItemService,
    private router: Router) { }

  ngOnInit() {
    console.log("ItemComponent instantiated with type " + this.class);

    var promise = null;
    switch(this.class) {
      default:
      case "latest":
        this.title = "Latest Items";
        promise = this.itemService.getLatest();
        break;
      case "most-viewed":
        this.title = "Most Viewed Items";
        promise = this.itemService.getMostViewed();
        break;
      case "random":
        this.title = "Random Items";
        promise = this.itemService.getRandom();
        break;
    }

    promise.then(items => this.items = items);
  }

  onSelect(item: Item) {
    this.selectedItem = item;
    console.log("Item with Id " + this.selectedItem.Id + "has been selected");
    this.router.navigate(['item', this.selectedItem.Id]);
  }
}