import { Component, OnInit, KeyValueDiffers } from '@angular/core';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  keyValues = [{ key: '', value: '' }];

  constructor() {

  }

  ngOnInit() {
  }

  add(placeHolder: string, val: string) {
    this.keyValues.push({ key: '', value: '' });

    this.keyValues.forEach(element => {
      console.log(element.key + ' ' + element.value);
    });
  }

  remove(index: number) {
    this.keyValues.splice(index, 1);
  }

}
