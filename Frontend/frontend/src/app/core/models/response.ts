export class Response<T> {
  results!: T;
  errors: Array<Error>;
  messages: Array<string>;
  constructor() {
    this.errors = [];
    this.messages = [];
  }
}

export class Error {
  code: string;
  description: string;
  type: string;
}