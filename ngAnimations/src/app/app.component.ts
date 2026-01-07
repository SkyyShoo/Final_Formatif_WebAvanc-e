import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shakeX, tada } from 'ng-animate';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true
})
export class AppComponent {
  title = 'ngAnimations';


  constructor() {
  }

  rotate= false;
  one = 0;
  two = 0;
  three = 0;

  rotateCube() {
    if(this.rotate == false){
      this.rotate = true;
      setTimeout(() => {this.rotate = false;},2000);
    }
  }

  playNgAnimations(forever:boolean) {
    this.one++;
    setTimeout(() => {
      this.two++;
      setTimeout(() => {
        this.three++;
        if(forever)
        {
          setTimeout(() => {
            this.playNgAnimations(true);
          },3000);
        }
      },3000);
    },2000);
  }
}
