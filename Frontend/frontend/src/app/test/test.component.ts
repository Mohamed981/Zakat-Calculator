import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';

@Component({
  selector: 'app-test',
  imports: [],
  templateUrl: './test.component.html',
  styleUrl: './test.component.scss',
})
export class TestComponent implements OnInit {
 
  constructor(private _user:UserService){}

  userList:any;

 ngOnInit(): void {
    this.userData();
  }

  userData(){
    this.userList = this._user.getData().subscribe(res=>{
      this.userList = res;
      console.log(this.userList);
    });
  }
}
