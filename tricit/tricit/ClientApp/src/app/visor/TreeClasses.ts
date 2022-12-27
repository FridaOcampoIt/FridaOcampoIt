import {CollectionViewer, SelectionChange} from '@angular/cdk/collections';
import {FlatTreeControl} from '@angular/cdk/tree';
import {Component, Injectable} from '@angular/core';
import {BehaviorSubject, merge, Observable} from 'rxjs';
import {map} from 'rxjs/operators';



export class DynamicFlatNode {
	constructor(public item: string, public productId: number, public level = 1, public expandable = false, public isLoading = false) {}
}

export class DynamicDatabase {
	dataMap = new Map<string, object[]>([
	
		['Onion', [['Yellow',4], ['White',54], ['Purple',546]]]
	]);
	
	rootLevelNodes: string[] = ['Fruits', 'Vegetables', 'extras'];

	/** Initial data from database */
	initialData(): DynamicFlatNode[] {
		return this.rootLevelNodes.map(name => new DynamicFlatNode(name, -1, 0, true));
	}

	getChildren(node: string): object[] | undefined {
		console.log("The node: " + node);
		console.log(this.dataMap.get(node));
		return this.dataMap.get(node);
	}

	isExpandable(node: string): boolean {
		return this.dataMap.has(node);
	}
}

@Injectable()
export class DynamicDataSource {

	dataChange = new BehaviorSubject<DynamicFlatNode[]>([]);

	get data(): DynamicFlatNode[] { return this.dataChange.value; }
	set data(value: DynamicFlatNode[]) {
		this.treeControl.dataNodes = value;
		this.dataChange.next(value);
	}

	constructor(private treeControl: FlatTreeControl<DynamicFlatNode>, private database: DynamicDatabase) {}

	connect(collectionViewer: CollectionViewer): Observable<DynamicFlatNode[]> {
		this.treeControl.expansionModel.onChange.subscribe(change => {
			console.log("treeControl.expansionModel.onChange");
			if ((change as SelectionChange<DynamicFlatNode>).added ||
			(change as SelectionChange<DynamicFlatNode>).removed) {
			this.handleTreeControl(change as SelectionChange<DynamicFlatNode>);
			}
		});

		return merge(collectionViewer.viewChange, this.dataChange).pipe(map(() => this.data));
	}

  /** Handle expand/collapse behaviors */
  handleTreeControl(change: SelectionChange<DynamicFlatNode>) {
	console.log("handleTreeControl()");
	console.log(change);
    if (change.added) {
      change.added.forEach(node => this.toggleNode(node, true));
    }
    if (change.removed) {
      change.removed.slice().reverse().forEach(node => this.toggleNode(node, false));
    }
  }

  /**
   * Toggle the node, remove from display list
   */
  toggleNode(node: DynamicFlatNode, expand: boolean) {
	console.log("toggleNode() " + expand);
	console.log(node);
	const children = this.database.getChildren(node.item);
	const index = this.data.indexOf(node);
	if (!children || index < 0) { // If no children, or cannot find the node, no op
		return;
	}

	node.isLoading = true;

	setTimeout(() => {
		console.log("setTimeout");
		console.log(children);
		if (expand) {
			var nodes = []
			for(var i in children)
				nodes.push(new DynamicFlatNode(children[i][0], children[i][1], node.level + 1, this.database.isExpandable(name)));
			//const nodes = children.map(name => new DynamicFlatNode(name, 0, node.level + 1, this.database.isExpandable(name)));
			this.data.splice(index + 1, 0, ...nodes);
		} else {
			let count = 0;
			for (let i = index + 1; i < this.data.length && this.data[i].level > node.level; i++, count++) {}
			this.data.splice(index + 1, count);
		}

		// notify the change
		this.dataChange.next(this.data);
		node.isLoading = false;
	}, 50);
	}
}
