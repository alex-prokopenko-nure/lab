import { Component } from '@angular/core';
import { APPService } from './services/app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  text: string = "";
  result: string = "";
  key: string = "";
  title = 'app';

  constructor (private _appService: APPService) {

  }

  encrypt = () => {
    this._appService.encrypt(this.text, this.key).subscribe(
      result => this.result = result
    );
  }

  decrypt = () => {
    this._appService.decrypt(this.text, this.key).subscribe(
      result => this.result = result
    );
  }
}
