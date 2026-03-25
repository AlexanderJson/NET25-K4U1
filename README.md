<p align="center">
  <img src="https://img.shields.io/badge/API-ASP.NET_Core-1f2937?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Database-SQLite-1f2937?style=for-the-badge&logo=sqlite" />
  <img src="https://img.shields.io/badge/Security-AES--256--GCM-darkslategray?style=for-the-badge&logo=shield" />
  <img src="https://img.shields.io/badge/Auth-JWT-374151?style=for-the-badge&logo=jsonwebtokens" />
</p>

<h1 align="center">Safe Storage</h1>

<p align="center">
  <em>A secure REST API for storing encrypted data</em><br/>
</p>

---

## Overview

A REST API in C# built with ASP.NET Core for securely storing and retrieving sensitive data.

The system is designed around the idea of **secrets**, where stored content can:

- expire after a certain time
- be limited to a certain number of views
- be protected with authenticated encryption
- be safely rejected if metadata or ciphertext has been tampered with

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
  - [Creating and Retrieving Secrets](#creating-and-retrieving-secrets)
  - [Authentication and User Access](#authentication-and-user-access)
- [Encryption & Integrity](#encryption--integrity)
- [Architecture](#architecture)
- [Security Design](#security-design)
- [Optimization](#optimization)
- [Running the Project](#running-the-project)
- [Future Improvements](#future-improvements)

---

## Features

### Encryption

Sensitive content is encrypted using **AES-256-GCM**, which provides both:

- **confidentiality** → the plaintext cannot be read without the key
- **integrity** → tampering causes decryption to fail

The encrypted output is stored as:

- **Nonce**
- **CipherText**
- **Tag**

In addition, metadata is bound using **AAD (Additional Authenticated Data)**.
Which is a recommended standard. This increases the integrity by cryptographically
tying metadata with the cipher. If any of these values are changed, decryption fails.

### Authentication

In addition to encryption, client can register as a user. 
Passwords are by default saved with ###Bcrypt###. 

JwT tokens are used for every endpoint exclusive to login,register and encrypt/decrypt with a valid 64-base token.


---

## Architecture

The project follows a layered structure:

```text
src/
├── Api/            # Controllers, middleware, startup/config
├── App/            # DTOs, services, interfaces, business logic
├── Domain/         # Core entities
├── Infrastructure/ # DbContext, repositories, persistence
```
Dependencies are centralised. Targets and scripting is used to ensure that
no vulnerable packages are imported to the project. Including packages that other libraries uses.


## Caching results

### Fetching 500 users 
This was tested with different amounts of data and for several iterations.

### Before caching
<img width="805" height="726" alt="3![Uploading Skärmbild 2026-03-25 022705.png…]()
23s" src="https://github.com/user-attachments/assets/fd1851d5-e37a-43e1-bafd-589743638b8d" />
### Afting caching
<img width="824" height="716" alt="19ms" src="https://github.com/user-attachments/assets/a6f5d9b1-2a29-4b18-b545-bdb2859e9241" />

