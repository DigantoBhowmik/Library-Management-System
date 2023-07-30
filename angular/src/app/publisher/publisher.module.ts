import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PublisherRoutingModule } from './publisher-routing.module';
import { PublisherComponent } from './publisher.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    PublisherComponent
  ],
  imports: [
    CommonModule,
    PublisherRoutingModule,
    SharedModule,
    NgbDatepickerModule
  ]
})
export class PublisherModule { }
