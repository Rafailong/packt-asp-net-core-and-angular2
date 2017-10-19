import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from '@angular/router';

import { Item } from "./item";
import { ItemService } from './item.service';

import { AuthService } from "./auth.service";

@Component({
  selector: "item-detail-edit",
  template: `
  <div *ngIf="item">
    <h2>
      <a href="#" (click)="onBack()">&laquo; Back to Home</a>
    </h2>
    <div class="item-container">
      <ul class="nav nav-tabs">
        <li *ngIf="authService.isLoggedIn()" role="presentation" class="active">
          <a href="javascript:void(0)" (click)="onUpdate(item)">Edit</a>
        </li>
        <li role="presentation" *ngIf="item.Id != 0">
          <a href="#" (click)="onItemDetailView(item)">View</a>
        </li>
      </ul>
      <div class="panel panel-default">
        <div class="panel-body">
          <form class="item-detail-edit">
            <h3>
              {{item.Title}}
              <span class="empty-field" [hidden]="dTitle.valid">Empty Title</span>
            </h3>
            <div class="form-group has-feedback" [ngClass]="{'has-success':dTitle.valid, 'has-error': !dTitle.valid}">
              <label for="input-title">Title</label>
              <input id="input-title" name="input-title" type="text" class="form-control"
                [(ngModel)]="item.Title" placeholder="Insert the title..." required #dTitle="ngModel" />
              <span class="glyphicon form-control-feedback" aria-hidden="true" 
                [ngClass]="{'glyphicon-ok': dTitle.valid, 'glyphicon-remove': !dTitle.valid}"></span>
              <div [hidden]=" dTitle.valid" class="alert alert-danger">
                You need to enter a valid Title.
              </div>
            </div>
            <div class="form-group">
              <label for="input-description">Description</label>
              <textarea id="input-description" name="inputdescription" class="form-control" [(ngModel)]="item.Description" placeholder="Insert a suitable description..." required></textarea>
            </div>
            <div class="form-group">
              <label for="input-text">Text</label>
              <textarea id="input-text" name="input-text" class="form-control" [(ngModel)]="item.Text" placeholder="Insert a suitable description..."></textarea>
            </div>
            <div *ngIf="item.Id == 0" class="commands insert">
              <input type="button" class="btn btn-primary" value="Save" (click)="onInsert(item)" />
              <input type="button" class="btn btn-default" value="Cancel" (click)="onBack()" />
            </div>
            <div *ngIf="item.Id != 0" class="commands update">
              <input type="button" class="btn btn-primary" value="Update" (click)="onUpdate(item)" />
              <input type="button" class="btn btn-danger" value="Delete" (click)="onDelete(item)" />
              <input type="button" class="btn btn-default" value="Cancel" (click)="onItemDetailView(item)" />
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
  `
})
export class ItemDetailEditComponent implements OnInit {
  item: Item;

  constructor(
    private router: Router,
    public authService: AuthService,
    private itemService: ItemService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate([""]);
    }

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
        this.router.navigate(['item', this.item.Id]);
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

  onItemDetailView(item: Item) {
    this.router.navigate(['item', item.Id]);
  }
}