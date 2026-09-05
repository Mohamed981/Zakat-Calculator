export class LoginPayload {
  // email: string;
  // password: string;
  returnUrl?: string;
}

export class LoginResult {
  accessToken: string;
  userId: number;
}
