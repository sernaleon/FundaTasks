<template>
  <v-dialog v-model="dialog" max-width="500px">
    <template v-slot:activator="{ on, attrs }">
      <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">
        New Task
      </v-btn>
    </template>
    <v-card>
      <v-card-title>
        <span class="headline">New Task</span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                label="Name*"
                v-model="newItem.name"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                label="Description"
                hint="optional"
                v-model="newItem.description"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
        <small>*indicates required field</small>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="close"> Cancel </v-btn>
        <v-btn color="blue darken-1" text @click="save"> Save </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop, Emit } from "vue-property-decorator";
import tasksApi, { Task, NewTask, UpdateTask } from "../api/tasks";

@Component
export default class NewItemDialog extends Vue {
  @Prop() dialog!: boolean;

  @Watch("dialog")
  onPropertyChanged(value: boolean) {
    console.log("propertyChanged!", value);
    if (!value) {
      this.close();
    }
  }

  created() {
    console.log("new item created");
  }

  public newItem: NewTask = {
    name: "",
    description: "",
  };

  private defaultItem: NewTask = {
    name: "",
    description: "",
  };

  editItem(item: Task) {
    this.newItem.groupId = item.groupId;
    this.newItem.name = item.name;
    this.newItem.description = item.description;
    this.dialog = true;
  }

  @Emit("updateTasks")
  async save(): Promise<Task[]> {
    const tasks = await tasksApi.add(this.newItem);
    this.close();
    return tasks;
  }

  async close() {
    this.dialog = false;
    await Vue.nextTick();
    this.newItem = Object.assign({}, this.defaultItem);
  }
}
</script>
