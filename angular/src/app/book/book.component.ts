import { BookWithAuthorDto } from './../proxy/book-with-authors/models';
import { BWAService } from './../proxy/book-with-authors/bwa.service';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookService, BookDto, bookTypeOptions } from '@proxy/books'; 
import { FormGroup, FormBuilder, Validators } from '@angular/forms'; 
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { AuthorDto, AuthorService } from '@proxy/authors';
import { PublisherService } from '@proxy/publishers';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss'],
  providers: [ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }]
})
export class BookComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;
  author = { items: [], totalCount: 0 } as PagedResultDto<AuthorDto>;
  publisher = { items: [], totalCount: 0 } as PagedResultDto<AuthorDto>;
  bwa = [];
  selectedBook = {} as BookDto;
  form: FormGroup; 

  bookTypes = bookTypeOptions;

  isModalOpen = false;
  isAModalOpen = false;

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder ,
    private authorService: AuthorService,
    private publisherService: PublisherService,
    private confirmation: ConfirmationService,
    private bwaservice: BWAService,
  ) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getList(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
    const authorStreamCreator = (query) => this.authorService.getList(query);

    this.list.hookToQuery(authorStreamCreator).subscribe((response) => {
      this.author = response;
      console.log(this.author)
    });
    const publisherStreamCreator = (query) => this.publisherService.getList(query);

    this.list.hookToQuery(publisherStreamCreator).subscribe((response) => {
      this.publisher = response;
      console.log(this.publisher)
    });
    
  }
  seeAuthor(id: string){
    this.isAModalOpen = true;
    this.bwaservice.getCustomAsynById(id).subscribe((bwaa)=>{
      this.bwa=bwaa;
    }
      
    )
  }

  createBook() {
    this.buildForm(); 
    this.selectedBook = {} as BookDto;;
    this.isModalOpen = true;
    console.log(this.form)
  }
  
  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.bookService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
  editBook(id: string) {
    this.bwaservice.getCustomAsynById(id).subscribe((bwaa)=>{
      this.bwa=bwaa;
      console.log(this.bwa);
    });
    this.bookService.get(id).subscribe((book) => {
      this.selectedBook = book;
      this.bwa.forEach(element=>{
        this.selectedBook.authors.push({
          id:element.authorId
        })
      })
      
      this.buildForm();
      this.isModalOpen = true;
      console.log(this.selectedBook)
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedBook.name || '', Validators.required],
      type: [this.selectedBook.type || null, Validators.required],
      publishDate: [
        this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
        Validators.required,
      ],
      price: [this.selectedBook.price || null, Validators.required],
      authors: [  this.selectedBook.authors || null, Validators.required],
      publisherId: [this.selectedBook.publisherId || null, Validators.required],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    const request = this.selectedBook.id
      ? this.bookService.updateCustom(this.selectedBook.id, this.form.value)
      : this.bookService.createBookCustom(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
