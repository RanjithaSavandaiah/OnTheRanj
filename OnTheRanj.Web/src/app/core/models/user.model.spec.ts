import { User, LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from './user.model';

describe('User Model', () => {
  it('should create a valid User object', () => {
    const user: User = {
      id: 1,
      fullName: 'Alice Smith',
      email: 'alice@example.com',
      role: 'Manager',
      isActive: true
    };
    expect(user.id).toBe(1);
    expect(user.fullName).toBe('Alice Smith');
    expect(user.email).toBe('alice@example.com');
    expect(user.role).toBe('Manager');
    expect(user.isActive).toBe(true);
  });
});

describe('LoginRequest Model', () => {
  it('should create a valid LoginRequest object', () => {
    const req: LoginRequest = {
      email: 'bob@example.com',
      password: 'secret123'
    };
    expect(req.email).toBe('bob@example.com');
    expect(req.password).toBe('secret123');
  });
});

describe('LoginResponse Model', () => {
  it('should create a valid LoginResponse object', () => {
    const res: LoginResponse = {
      token: 'jwt-token',
      message: 'Login successful'
    };
    expect(res.token).toBe('jwt-token');
    expect(res.message).toBe('Login successful');
  });
});

describe('RegisterRequest Model', () => {
  it('should create a valid RegisterRequest object', () => {
    const req: RegisterRequest = {
      fullName: 'Charlie Brown',
      email: 'charlie@example.com',
      password: 'pass123',
      role: 'Employee'
    };
    expect(req.fullName).toBe('Charlie Brown');
    expect(req.email).toBe('charlie@example.com');
    expect(req.password).toBe('pass123');
    expect(req.role).toBe('Employee');
  });
});

describe('RegisterResponse Model', () => {
  it('should create a valid RegisterResponse object', () => {
    const res: RegisterResponse = {
      userId: 2,
      fullName: 'Dana White',
      email: 'dana@example.com',
      role: 'Manager',
      message: 'Registration successful'
    };
    expect(res.userId).toBe(2);
    expect(res.fullName).toBe('Dana White');
    expect(res.email).toBe('dana@example.com');
    expect(res.role).toBe('Manager');
    expect(res.message).toBe('Registration successful');
  });
});
