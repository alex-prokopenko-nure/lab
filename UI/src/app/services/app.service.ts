import { Injectable } from '@angular/core';
import { APIService } from './api.service';

@Injectable()
export class APPService {
    constructor(private _apiService: APIService) {
    }

    encrypt = (input: string, key: string) => {
        return this._apiService.apiCryptoEncryptGet(input, key);
    }

    decrypt = (input: string, key: string) => {
        return this._apiService.apiCryptoDecryptGet(input, key);
    }
}