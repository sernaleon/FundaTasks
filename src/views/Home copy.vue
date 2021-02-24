<template>
  <v-container>
    <v-card class="mx-auto" elevation="2" max-width="800">
      <v-btn color="primary" @click="start()">GET</v-btn>
      <v-btn color="primary" @click="add()">ADD</v-btn>
      <v-data-table
        :headers="headers"
        :items="tasks"
        sort-by="timestamp"
        class="elevation-1"
      >
        <NewItemDialog :dialog="dialog" @tasks="receivedStuff" />
        <template v-slot:[`item.actions`]="{ item }">
          <v-icon small class="mr-2" @click="addFromExisting(item)">
            mdi-plus
          </v-icon>
        </template>
        <template v-slot:no-data>
          <v-btn color="primary" @click="start()"> Sign in to get data </v-btn>
        </template>
      </v-data-table>
    </v-card>
  </v-container>
</template> 

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import tasksApi, { Task, NewTask, UpdateTask } from "../api/tasks";
import NewItemDialog from "./NewItemDialog.vue";

@Component({
  components: { NewItemDialog },
})
export default class Home extends Vue {
  public tasks: Task[] = [];
  public dialog = false;
  public headers = [
    {
      text: "Name",
      align: "start",
      sortable: true,
      value: "task.name",
    },
    { text: "Description", value: "description" },
    { text: "Actions", value: "actions", sortable: false },
  ];

  receivedStuff(tasks: Task[]) {
    console.log("received", tasks);
    this.tasks = tasks;
  }

  updateTasks(tasks: Task[]) {
    console.log("received", tasks);
    this.tasks = tasks;
  }

  add() {
    this.dialog = true;
    console.log("add", this.dialog);
  }

  addFromExisting(task: Task) {
    console.log("add", task);
  }

  async created() {
    await this.getTasksAsync();
  }

  async start() {
    if (!this.$msal.isAuthenticated) {
      await this.$msal.signIn();
    }
    await this.getTasksAsync();
  }

  async getTasksAsync() {
    if (this.$msal.isAuthenticated) {
      this.tasks = await tasksApi.get();
    }
  }
}
</script>
