export class GenerateProjectRequest {
    projectName: string;
    repositoryLink: string;
    renamePairs: KeyValuePair<string,string>[];
    branchName: string;
    userName: string;
    password: string;
}

export class KeyValuePair<T1, T2>
{
    key: T1;
    value: T2;
}