import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PublisherDto, PublisherService } from '@proxy/publishers';

@Component({
  selector: 'app-publisher',
  templateUrl: './publisher.component.html',
  styleUrls: ['./publisher.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class PublisherComponent implements OnInit {

  publisher = { items: [], totalCount: 0 } as PagedResultDto<PublisherDto>;

  isModalOpen = false;

  form: FormGroup;

  selectedpublisher = {} as PublisherDto;

  constructor(
    public readonly list: ListService,
    private publisherService: PublisherService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    const publisherStreamCreator = (query) => this.publisherService.getList(query);

    this.list.hookToQuery(publisherStreamCreator).subscribe((response) => {
      this.publisher = response;
      console.log(this.publisher)
    });
  }

  createpublisher() {
    this.selectedpublisher = {} as PublisherDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editpublisher(id: string) {
    this.publisherService.get(id).subscribe((publisher) => {
      this.selectedpublisher = publisher;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedpublisher.name || '', Validators.required]
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    if (this.selectedpublisher.id) {
      this.publisherService
        .update(this.selectedpublisher.id, this.form.value)
        .subscribe(() => {
          this.isModalOpen = false;
          this.form.reset();
          this.list.get();
        });
    } else {
      this.publisherService.create(this.form.value).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure')
        .subscribe((status) => {
          if (status === Confirmation.Status.confirm) {
            this.publisherService.delete(id).subscribe(() => this.list.get());
          }
	    });
  }

}
