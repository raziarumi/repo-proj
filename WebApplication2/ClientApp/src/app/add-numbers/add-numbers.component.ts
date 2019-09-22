import { Component, OnInit } from '@angular/core';
import { MatInputModule } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppService } from '../app-service';

@Component({
  selector: 'app-add-numbers',
  templateUrl: './add-numbers.component.html',
  styleUrls: ['./add-numbers.component.css']
})
export class AddNumbersComponent implements OnInit {

  submitted = false;
  summationForm: FormGroup;
  summationData: any;
  summation = "";
  constructor(private formBuilder: FormBuilder, private appService: AppService) { }

  ngOnInit() {
    this.buildForm();
  }
  onSubmit() {
    this.submitted = true;
    const formData = new FormData();
    this.summationData = this.summationForm.value;
    // stop here if form is invalid
    if (this.summationForm.invalid) {
      return;
    }
    this.appService.sum(this.summationForm.value).subscribe((data: any) => {
      this.summation = data.summation;
      //this.summationForm.reset();
    });
  }
  buildForm() {
    this.summationForm = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.maxLength(250)]],
      number1: ['', [Validators.required]],//, Validators.pattern('^[0-9]*$')]],
      number2: ['', [Validators.required]],//, Validators.pattern('^[0-9]*$')]],
    });
  }

  get f() { return this.summationForm.controls; }


}
