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

A REST API in C# built with ASP.NET Core for securely storing and retrieving sensitive text.

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

### Creating and Retrieving Secrets

- **Create encrypted secrets**
- **Retrieve stored secrets**
- **Automatic expiration handling**
- **View limit enforcement**
- **Optional password-protected access**

<p align="center">
  <b>Secret creation flow</b><br/>
  <img src="docs/create-secret.png" width="80%" />
</p>

<p align="center">
  <b>Secret retrieval flow</b><br/>
  <img src="docs/get-secret.png" width="80%" />
</p>

<p align="center">
  <b>View / expiration enforcement</b><br/>
  <img src="docs/secret-rules.png" width="80%" />
</p>

---

### Authentication and User Access

- **JWT-based authentication**
- **Authenticated user endpoints**
- **User-specific secret actions**
- **Secure identity handling through claims**

<p align="center">
  <b>Authenticated user flow</b><br/>
  <img src="docs/auth-flow.png" width="80%" />
</p>

---

## Encryption & Integrity

Sensitive content is encrypted using **AES-256-GCM**, which provides both:

- **confidentiality** → the plaintext cannot be read without the key
- **integrity** → tampering causes decryption to fail

The encrypted output is stored as:

- **Nonce**
- **CipherText**
- **Tag**

In addition, metadata is bound using **AAD (Additional Authenticated Data)**.

That means values such as:

- `SecretId`
- `ExpiresAt`
- `MaxViews`
- `RequiresPassword`

are not encrypted, but they are still cryptographically tied to the ciphertext.

If any of these values are changed, decryption fails.

<p align="center">
  <b>AES-GCM structure</b><br/>
  <img src="docs/encryption-flow.png" width="80%" />
</p>

---

## Architecture

The project follows a layered structure:

```text
src/
├── Api/            # Controllers, middleware, startup/config
├── App/            # DTOs, services, interfaces, business logic
├── Domain/         # Core entities
├── Infrastructure/ # DbContext, repositories, persistence
