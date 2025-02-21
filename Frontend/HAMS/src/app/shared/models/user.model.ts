export class User {
  userId?: number;
  fullName!: string;
  email!: string;
  password?: string; 
  role!: string;
}

export class Login {
  email!: string;
  password!: string;
  jwtTokenKey?: string;
  fullName?: string; 
}

export class Register {
  fullName!: string;
  email!: string;
  password!: string;
  role!: string;
}

export class ChangePassword {
  email!: string;
  // currentPassword!: string;
  newPassword!: string;
}
  