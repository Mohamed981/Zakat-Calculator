export class LoginPayload {
  email: string;
  password: string;
}

export class CurrentUser {
  userId: number;
  email: string | null;
  name: string | null;
}
