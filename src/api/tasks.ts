import { msalPluginInstance } from "@/plugins/msal-plugin";
import axios, { AxiosRequestConfig } from "axios";

export interface Task {
    id: string;
    groupId: string;
    name: string;
    description: string;
    timestamp: Date;
}

export interface NewTask {
    groupId?: string;
    name: string;
    description: string;
}

export interface UpdateTask {
    id: string;
    groupId: string;
    name: string;
    description: string;
}


class TasksApi {
    async get(): Promise<Task[]> {
        try {
            const config = await this.getAuthorisedConfigAsync();
            const response = await axios.get('/api/tasks', config);

            return response.data;
        } catch (e) {
            console.error(e);
            return [];
        }
    }

    async add(item: NewTask): Promise<Task[]> {
        try {
            const config = await this.getAuthorisedConfigAsync();
            const response = await axios.post('/api/tasks', item, config);

            return response.data;
        } catch (e) {
            console.error(e);
            return [];
        }
    }

    async update(item: NewTask): Promise<Task[]> {
        try {
            const config = await this.getAuthorisedConfigAsync();
            const response = await axios.put('/api/tasks', item, config);

            return response.data;
        } catch (e) {
            console.error(e);
            return [];
        }
    }

    async delete(id: string): Promise<Task[]> {
        try {
            const config = await this.getAuthorisedConfigAsync();
            const response = await axios.delete(`/api/tasks/${id}`, config);

            return response.data;
        } catch (e) {
            console.error(e);
            return [];
        }
    }

    private async getAuthorisedConfigAsync(): Promise<AxiosRequestConfig> {
        const accessToken = await msalPluginInstance.acquireToken();

        const config = {
            headers: { 'Authorization': 'Bearer ' + accessToken }
        };

        return config;
    }
}

export default new TasksApi();