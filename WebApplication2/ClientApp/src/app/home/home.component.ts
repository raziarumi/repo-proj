import { Component, OnInit, ViewChild, EventEmitter, Output } from '@angular/core';
import { AppService } from '../app-service';
import { Sort, MatSort, MatDatepickerInputEvent } from '@angular/material';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  @Output() dateChange: EventEmitter<MatDatepickerInputEvent<any>>
  results: ResultData[];
  resultsCopy: ResultData[];
  userNameFilterText = '';
  fromDateFilterText: any;
  toDateFilterText: any;
  @ViewChild(MatSort) sort: MatSort;
  constructor(private appService: AppService) { }

  ngOnInit() {
    this.getResults();
  }
  getResults() {
    this.appService.getResults().subscribe((data: any) => {
      this.results = data;
      this.resultsCopy = data;
    });
  }
  sortData(sort: Sort) {
    const data = this.results.slice();
    if (!sort.active || sort.direction === '') {
      //this.results = this.results ;
      return;
    }

    this.results = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'createdDate': return this.compareDate(a.createdDate, b.createdDate, isAsc);
        case 'userName': return this.compare(a.userName, b.userName, isAsc);
        case 'number1': return this.compareNumber(a.number1, b.number1, isAsc);
        case 'number2': return this.compareNumber(a.number2, b.number2, isAsc);
        case 'sum': return this.compareNumber(a.sum, b.sum, isAsc);
        default: return 0;
      }
    });
  }

  compareDate(a: string, b: string, isAsc: boolean) {

    return (Date.parse(a) < Date.parse(b) ? -1 : 1) * (isAsc ? 1 : -1);
  }
  compareNumber(a: string, b: string, isAsc: boolean) {
    let diff = 0;
    var isANeg = a.indexOf('-') > -1;
    var isBNeg = b.indexOf('-') > -1;

    if (isANeg) {
      a = a.replace("-", "");
    }
    if (isBNeg) {
      b = b.replace("-", "");
    }

    var aScale = a;
    var aPrecision = "";
    var bScale = b;
    var bPreision = "";
    if (a.indexOf('.') > -1) {
      var arr = a.split('.');
      aScale = arr[0];
      aPrecision = arr[1];
    }
    if (b.indexOf('.') > -1) {
      var arr = b.split('.');
      bScale = arr[0];
      bPreision = arr[1];
    }
    if (aPrecision.length < bPreision.length) {
      aPrecision = aPrecision.padEnd(bPreision.length, '0');
    }
    if (bPreision.length < aPrecision.length) {
      bPreision = aPrecision.padEnd(aPrecision.length, '0');
    }

    if (aScale.length < bScale.length) {
      aScale = aScale.padStart(bScale.length, '0');
     
      //if (aPrecision != "")
      //  a = a + '.' + aPrecision;
    }
    else {
      bScale = bScale.padStart(aScale.length, '0')
      
      //if (bPreision != "")
      //  b = b + '.' + bPreision;
    }
    a = aScale;
    if (aPrecision != "")
      a = a + '.' + aPrecision;
    b = bScale;
    if (bPreision != "")
      b = b + '.' + bPreision;

    if (isANeg) { a = '-' + a; }
    if (isBNeg) { b = '-' + b; }
    
    debugger;
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
  compare(a: number | string, b: number | string, isAsc: boolean) {

    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
  orgValueChange(fromDate: any) {

  }
  applyFilteronResult(fromdate: string, todate: string, userName: string) {

    this.results = this.resultsCopy.filter(item => (!userName || item.userName.toLowerCase().indexOf(userName.toLowerCase()) > - 1)
      && (!fromdate || Date.parse(item.createdDate) >= Date.parse(fromdate)) && (!todate || Date.parse(todate) >= Date.parse(item.createdDate))
    );

  }
  applyFilterName(filterValue: string) {
    this.userNameFilterText = filterValue;
    this.applyFilteronResult(this.fromDateFilterText, this.toDateFilterText, this.userNameFilterText);
    //const tableFilters = [];
    //tableFilters.push({
    //  id: 'userName',
    //  value: filterValue
    //});
    //if (!filterValue) {
    //  this.results = this.resultsCopy;
    //}
    //this.results = this.results.filter(item => item.userName.toLowerCase().indexOf(filterValue.toLowerCase()) > - 1);
  }
  orgFromDateValueChange(fromDate: any) {

    this.fromDateFilterText = fromDate;
    this.applyFilteronResult(this.fromDateFilterText, this.toDateFilterText, this.userNameFilterText);
  }

  orgToDateValueChange(toDate: any) {
    this.toDateFilterText = toDate;
    if (toDate) {
      var d = new Date(toDate)
      d.setHours(23);
      d.setMinutes(59);
      d.setSeconds(59);
      this.toDateFilterText = d;
    }
    this.applyFilteronResult(this.fromDateFilterText, this.toDateFilterText, this.userNameFilterText);
  }
}
export interface ResultData {
  createdDate: string;
  userName: string;
  number1: string;
  number2: string;
  sum: string;
}

