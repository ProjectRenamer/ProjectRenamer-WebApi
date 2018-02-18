import { Component, OnInit, KeyValueDiffers } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { environment } from '@env/environment';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  projectUrl: string;
  keyValues = [];
  currentKV = { key: '', value: '' };
  projectName = '';

  constructor(private httpClient: HttpClient) {

  }

  ngOnInit() {
  }

  add(placeHolder: string, val: string) {
    this.keyValues.push({ key: placeHolder, value: val });
    this.currentKV = { key: '', value: '' };
  }

  remove(index: number) {
    this.keyValues.splice(index, 1);
  }

  generate() {

    this.httpClient.post(environment.DotNetTemplateUrl + '/generator/',
      {
        'projectName': this.projectName,
        'repositoryLink': this.projectUrl,
        'renamePairs': this.keyValues
      },
      {
        responseType: 'blob'
      }
    )
      .subscribe((response) => {
        const blob = new Blob([response], { type: 'application/zip' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
        err => {
          throw err;
        });
  }

}
