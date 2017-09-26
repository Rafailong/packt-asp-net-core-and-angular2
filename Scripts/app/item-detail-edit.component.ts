import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from '@angular/router';

import { Item } from "./item";
import { ItemService } from './item.service';

@Component({
  selector: "item-detail-edit",
  template: `
    <div *ngIf="item" class="item-details">
      <h2>{{item.Title}} - Detail View</h2>
      <ul>
        <li>
          <label>Title:</label>
          <input [(ngModel)]="item.Title" placeholder="Insert the title..." />
        </li>
        <li>
          <textarea [(ngModel)]="item.Description" placeholder="Insert a suitable description">
          </textarea>
        </li>
      </ul>
      <div *ngIf="item.Id <= 0">
        <input type="button" value="Save" (click)="onInsert(item)" />
        <input type="button" value="Cancel" (click)="onBack()" />
      </div>
      <div *ngIf="item.Id != 0" class="commands update">
        <input type="button" value="Update" (click)="onUpdate(item)" />
        <input type="button" value="Delete" (click)="onDelete(item)" />
        <input type="button" value="Back" (click)="onBack()" />
    </div>
  `,
  styles: [`
    .item-details {
      margin: 5px;
      padding: 5px 10px;
      border: 1px solid black;
      background-color: #dddddd;
      width: 300px;
    }
    .item-details * {
      vertical-align: middle;
    }
    .item-details ui li {
      padding: 5px 0;
    }
  `]
})
export class ItemDetailEditComponent implements OnInit {
  item: Item;

  constructor(
    private router: Router,
    private itemService: ItemService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    var id = Number.parseInt(this.activatedRoute.snapshot.params['id']);
    if (id > 0) {
      this.itemService.get(id)
        .then(item => this.item = item);
    } else if (id === 0) {
      console.log("adding a new item...");
      this.item = new Item(0, 'New Item', null);
    } else {
      this.router.navigate(['']);
    }
  }

  onInsert(item: Item) {
    this.itemService.add(item).subscribe(
      (data) => {
        this.item = data;
        console.log('item added!');
        this.router.navigate(['']);
      },
      (err) => console.error(err)
    )
  }

  onBack() {
    this.router.navigate(['']);
  }

  onUpdate(item: Item) {
    this.itemService.update(item).subscribe(
      (data) => {
        this.item = data;
        console.log('item updated!');
        this.router.navigate(['']);
      },
      (err) => console.error(err)
    )
  }

  onDelete(item: Item){
    this.itemService.delete(item.Id).subscribe(
      (data) => {
        this.item = data;
        console.log('item deleted!');
        this.router.navigate(['']);
      },
      (err) => console.error(err)
    )
  }
}